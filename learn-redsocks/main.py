import socket
import struct

# Constants for socket option
SO_ORIGINAL_DST = 80
SOL_IP = 0

def get_original_destination(conn):
    # Get the original destination address and port
    packed_addr = conn.getsockopt(SOL_IP, SO_ORIGINAL_DST, 16)
    _, port, raw_ip = struct.unpack('!HH4s8x', packed_addr)
    ip = socket.inet_ntoa(raw_ip)
    return ip, port

def main():
    # Example: Create a server socket
    server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server_socket.bind(('0.0.0.0', 12345))
    server_socket.listen(5)

    print("Listening for connections...")

    while True:
        conn, addr = server_socket.accept()
        print(f"Connection from {addr}")

        # Get the original destination
        original_ip, original_port = get_original_destination(conn)
        print(f"Original destination: {original_ip}:{original_port}")

        # Close the connection
        conn.close()

if __name__ == "__main__":
    main()