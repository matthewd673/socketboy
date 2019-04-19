# SocketBoy
Helps you with sockets

## Commands
* **/connect [ip:port]** - connect to the specified server (ip defaults to localhost, port defaults to 80)
* **/disconnect** - disconnect from the current server
* **/hi** - politely greet the server
* **/wait** [time] - wait for the specified time (ms) (time defaults to 1000ms)
* **/load** [script] - load and execute specified script file

## Example Script
```
/connect :1234
/wait 2000
hello there, server
/wait 1000
/disconnect
```