import { WebSocketServer } from 'ws';

const wss = new WebSocketServer({ port: process.argv[2] });

wss.on('connection', function connection(ws) {
  ws.on('error', console.error);

  ws.on('message', function message(data) {
    console.log('received: %s', data);
    ws.send(`received: ${data}`);
  });

  ws.send('something');
});