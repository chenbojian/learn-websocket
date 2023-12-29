# learn-websocket
learn websocket


- [websocket from scratch](https://gist.github.com/tabjy/5b7d2c549cb29d9ac07a28aad1c9bfa4)
- 使用haproxy [官方websocket loadbalancer配置](https://www.haproxy.com/documentation/haproxy-configuration-tutorials/load-balancing/websocket/)，当服务端下线客户端会被close，客户端需要实现重新连接
- socket.io支持自动重连，但是loadbalancer需要配置sticky session，roundrobin会导致无法连上，[nginx配置例子](https://socket.io/docs/v4/using-multiple-nodes/#nginx-configuration)