var io = require('socket.io')(process.env.PORT || 52300);

// Custom Classes
var Player = require('./Classes/Player.js');

console.log('Server has started');

var players = new Map();
var sockets = new Map();

io.on('connection', function(socket) {

    console.log('Connection Made!');

    var player = new Player();
    var thisPlayerId = player.id;

    players.set(thisPlayerId, player);
    sockets.set(thisPlayerId, socket);

    // Tell the client that this is our id for the server
    socket.emit('register', {id: thisPlayerId});

    socket.emit('spawn', player); // Tell myseld i have spawned
    socket.broadcast.emit('spawn', player);

    console.log(players);
    // Tell myself about everybody else in the game
    players.forEach(function(value, key, map){
        if(key != thisPlayerId){
            console.log("Spawn enemy");
            socket.emit('spawn', value);
        }
    });

    // Positional Data from Client
    socket.on('updatePosition', function(data){
        console.log("DATA - " + JSON.stringify(data));

        player.position.x = data.position.x;
        player.position.y = data.position.y;
        player.position.z = data.position.z;

        socket.broadcast.emit('updateClientPosition', player);
    });

    socket.on('disconnect', function(){
        console.log('Player has disconnect');
        players.delete(thisPlayerId);
        sockets.delete(thisPlayerId);
        socket.broadcast.emit('disconnectClient', player);
    });

});