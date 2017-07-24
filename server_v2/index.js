"use strict";  
var WebSocketServer = require('ws').Server;
var express = require('express');
var path = require('path');
var app = express.createServer();
var uuid = require('uuid');






app.use(express.static(path.join(__dirname, '/public')));
app.listen(8080);

let sockets = [];
let nicknames = [];

var wss = new WebSocketServer({ server: app });





wss.on('connection', function (ws) {
  "use strict";  



  let wsId = uuid.v1();
  sockets[wsId] = ws;
  console.log("client connected");

  ws.on('close', function () {
    delete sockets[wsId];
    console.log('client disconnected');
  });

  ws.on('message', function incoming(message) {
    
    // Deserialize the JSON object
    var msgJson = JSON.parse(message);
    var name = msgJson.name;
    var msg = msgJson.msg;
    var type = msgJson.type;
    
    // Test identification or message
    switch(type){
      case ("id") :
        nicknames[wsId] = name;
        console.log("client name = "+name);
        break;
      case ("msg") :
        console.log(msg);
        for (let id in sockets) {
          //if (id !== wsId) {
          if (nicknames[id] !== name) {
            sockets[id].send(message);
          }
        }
        break;
    }



    
    
  });
});
