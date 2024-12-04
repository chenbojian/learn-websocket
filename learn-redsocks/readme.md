1. install `apt-get install iptables telnet`
2. use `iptables -t nat -A OUTPUT -d 10.18.1.1 -p tcp --dport 80 -j REDIRECT --to-port 12345` to redirect to python listen port
3. use `telnet 10.18.1.1 80` to view output
