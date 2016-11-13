var WebSocketServer = require('ws').Server;
var express = require('express');
var path = require('path');
var app = express.createServer();
var uuid = require('uuid');


app.use(express.static(path.join(__dirname, '/public')));
app.listen(8080);

let sockets = [];

var wss = new WebSocketServer({ server: app });
console.log(wss);
wss.on('connection', function (ws) {
  let wsId = uuid.v1();
  sockets[wsId] = ws;
  console.log("client connected");
  ws.on('close', function () {
    delete sockets[wsId];
    console.log('client disconnected');
  });
  ws.on('message', function incoming(message) {
    for (let id in sockets) {
      // if (id !== wsId) {
        ws.send(message);
      // }
    }
    console.log('received:', message);
  });
});