package com.tfg.freeling.services;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.net.Socket;
import java.nio.charset.StandardCharsets;

public class FreelingService {

    private static final String FREELING_HOST = "my-freeling";
    private static final int FREELING_PORT = 50005;

    private static final String MESSAGE_SERVER_READY = "FL-SERVER-READY";
    private static final String MESSAGE_RESET_STATS = "RESET_STATS";
    private static final String MESSAGE_FLUSH_BUFFER = "FLUSH_BUFFER";

    private final DataInputStream dataInputStream;
    private final DataOutputStream dataOutputStream;

    public FreelingService() throws IOException {
        Socket socket = new Socket(FREELING_HOST, FREELING_PORT);
        socket.setSoLinger(true, 10);
        socket.setKeepAlive(true);
        socket.setSoTimeout(10000);

        dataInputStream = new DataInputStream(socket.getInputStream());
        dataOutputStream = new DataOutputStream(socket.getOutputStream());

        writeMessage(MESSAGE_RESET_STATS);
        StringBuffer sb = readMessage();
        String message = sb.toString().replaceAll("\0", "");
        if (message.compareTo(MESSAGE_SERVER_READY) != 0) {
            throw new IOException("Server not ready");
        }

        writeMessage(MESSAGE_FLUSH_BUFFER);
        readMessage();
    }

    public String processSegment(String text) throws IOException {
        writeMessage(text);
        StringBuffer sb = readMessage();

        writeMessage(MESSAGE_FLUSH_BUFFER);
        readMessage();
        return sb.toString().replaceAll("\0", "");
    }

    private void writeMessage (String message) throws IOException {
        dataOutputStream.write(message.getBytes(StandardCharsets.UTF_8));
        dataOutputStream.write(0);
        dataOutputStream.flush();
    }

    private StringBuffer readMessage() throws IOException {
        byte[] buffer = new byte[2048];
        int bl;
        StringBuffer sb = new StringBuffer();

        do {
            bl = dataInputStream.read(buffer, 0, 2048);
            if (bl > 0) {
                sb.append(new String(buffer, 0 , bl));
            }
        } while (bl > 0 && buffer[bl - 1] != 0);

        return sb;
    }
}
