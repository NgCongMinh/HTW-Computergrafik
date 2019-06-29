var shortId = require('shortid');
var Vector3 = require('./Vector3.js');

module.exports = class Player {
    constructor(){
        this.id = shortId.generate();
        this.position = new Vector3();
    }
}