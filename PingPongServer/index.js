var io = require('socket.io')(process.env.PORT || 52300);

// constants
var MAX_PLAYER_NUMBER = 2;

// Custom Classes
var Player = require('./Classes/Player.js');

console.log('Server has started');

var players = new Map();
var sockets = new Map();

io.on('connection', function(socket) {

    console.log('Connection Made!');

    var player = new Player();
    var thisPlayerId = player.id;

    var playerCount = players.size;
    console.log("PLAYERS : " + playerCount);
    if (playerCount == MAX_PLAYER_NUMBER) {
        // disconnect
        socket.emit('playerRejected', {});
    } else {
        players.set(thisPlayerId, player);
        sockets.set(thisPlayerId, socket);

        // Tell the client that this is our id for the server
        socket.emit('register', {id: thisPlayerId});

        // Tell myseld i have spawned
        socket.emit('spawn', player);
        
        // tell everyone else I spawned
        socket.broadcast.emit('spawn', player);

        console.log(players);
        // Tell myself about everybody else in the game
        players.forEach(function(value, key, map){
            if(key != thisPlayerId){
                console.log("Spawn enemy");
                socket.emit('spawn', value);
            }
        });
    }

    // Positional Data from Client
    socket.on('updatePosition', function(data){
        //console.log("DATA - " + JSON.stringify(data));

        player.position.x = data.position.x;
        player.position.y = data.position.y;
        player.position.z = data.position.z;

        socket.broadcast.emit('updateClientPosition', player);
    });

    socket.on('spawnBall', function(data){
        //console.log("DATA - " + JSON.stringify(data));

        socket.emit('updateBallPosition', data);
        socket.broadcast.emit('updateBallPosition', data);
    });

    socket.on('disconnect', function(){
        console.log('Player has disconnect');
        players.delete(thisPlayerId);
        sockets.delete(thisPlayerId);
        socket.broadcast.emit('disconnectClient', player);
    });

});