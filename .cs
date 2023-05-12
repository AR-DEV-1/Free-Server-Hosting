import java.net.ServerSocket;
import java.net.Socket;
import java.util.ArrayList;

public class ServerManager {
    private ArrayList<ServerInstance> servers = new ArrayList<>();

    public void createServer(String name, int port) {
        ServerInstance server = new ServerInstance(name, port);
        servers.add(server);
        server.start();
    }

    public void stopServer(String name) {
        for (ServerInstance server : servers) {
            if (server.getName().equals(name)) {
                server.stop();
                servers.remove(server);
                break;
            }
        }
    }

    public void listServers() {
        for (ServerInstance server : servers) {
            System.out.println(server.getName() + " - " + server.getAddress() + ":" + server.getPort());
        }
    }

    public static void main(String[] args) {
        ServerManager manager = new ServerManager();

        // Sample usage
        manager.createServer("MyServer", 12345);
        manager.listServers();
        manager.stopServer("MyServer");
    }
}

class ServerInstance extends Thread {
    private String name;
    private int port;
    private ServerSocket serverSocket;
    private ArrayList<Socket> clients = new ArrayList<>();

    public ServerInstance(String name, int port) {
        this.name = name;
        this.port = port;
    }

    public String getName() {
        return name;
    }

    public String getAddress() {
        return serverSocket.getInetAddress().getHostAddress();
    }

    public int getPort() {
        return port;
    }

    public void run() {
        try {
            serverSocket = new ServerSocket(port);

            while (true) {
                Socket clientSocket = serverSocket.accept();
                clients.add(clientSocket);
                new ClientHandler(clientSocket, this).start();
            }
        } catch (Exception e) {
            System.out.println("Server error: " + e.getMessage());
        }
    }

    public void stop() {
        try {
            serverSocket.close();
            for (Socket client : clients) {
                client.close();
            }
        } catch (Exception e) {
            System.out.println("Error stopping server: " + e.getMessage());
        }
    }
}

class ClientHandler extends Thread {
    private Socket clientSocket;
    private ServerInstance serverInstance;

    public ClientHandler(Socket clientSocket, ServerInstance serverInstance) {
        this.clientSocket = clientSocket;
        this.serverInstance = serverInstance;
    }

    public void run() {
        // Handle client connection
    }
}
