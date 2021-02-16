import com.google.gson.Gson;

import com.rabbitmq.client.Channel;
import com.rabbitmq.client.Connection;
import com.rabbitmq.client.ConnectionFactory;
import com.rabbitmq.client.DeliverCallback;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.net.Socket;

public class Worker {

    private static final String FREELING_HOST = "localhost";
    private static final int FREELING_PORT = 50005;
    private static final int BUF_SIZE = 2048;
    private static final String SERVER_READY_MSG = "FL-SERVER-READY";
    private static final String RESET_STATS_MSG = "RESET_STATS";
    private static final String ENCODING = "UTF8";
    private static final String FLUSH_BUFFER_MSG = "FLUSH_BUFFER";

    private final static String QUEUE_NAME = "tfg-queue";
    private final static String TASK_QUEUE_NAME = "tfg-queue";
    private final static String RABBITMQ_HOST = "localhost";

    private static Socket socket;
    private static DataInputStream inputBuffer;
    private static DataOutputStream outputBuffer;

    public static void main(String[] args) throws Exception {
        ConnectionFactory factory = new ConnectionFactory();
        factory.setHost(RABBITMQ_HOST);
        final Connection connection = factory.newConnection();
        final Channel channel = connection.createChannel();

        channel.basicQos(1);

        channel.queueDeclare(QUEUE_NAME, false, false, false, null);
        DeliverCallback deliverCallback = (consumerTask, delivery) -> {
            try {
                String json = new String(delivery.getBody(), "UTF-8");
                doWork(json);
            } catch (Exception e) {
                e.printStackTrace();
            } finally {
                channel.basicAck(delivery.getEnvelope().getDeliveryTag(), false);
            }
        };

        channel.basicConsume(TASK_QUEUE_NAME, true, deliverCallback, consumerTag -> {});
    }

    private static void doWork(String jsonTask) throws Exception {
        System.out.println(jsonTask);

        TextData data = new Gson().fromJson(jsonTask, TextData.class);

        socket = new Socket(FREELING_HOST, FREELING_PORT);
        socket.setSoLinger(true, 10);
        socket.setKeepAlive(true);
        socket.setSoTimeout(10000);

        inputBuffer = new DataInputStream(socket.getInputStream());
        outputBuffer = new DataOutputStream(socket.getOutputStream());

        writeMessage(outputBuffer, RESET_STATS_MSG, ENCODING);

        StringBuffer sb = readMessage(inputBuffer);
        String message = sb.toString().replaceAll("\0", "");
        if (message.compareTo(SERVER_READY_MSG) != 0) {
            System.err.println("SERVER NOT READY");
        }

        writeMessage(outputBuffer, FLUSH_BUFFER_MSG, ENCODING);
        readMessage(inputBuffer);

        String response = processSegment(data.getText());

    }

    private static void writeMessage(DataOutputStream out, String message, String encoding) throws IOException {
        out.write(message.getBytes(encoding));
        out.write(0);
        out.flush();
    }

    private static synchronized StringBuffer readMessage(DataInputStream inputBuffer) throws IOException {
        byte[] buffer = new byte[BUF_SIZE];
        int bl;
        StringBuffer sb = new StringBuffer();

        do {
            bl = inputBuffer.read(buffer, 0, BUF_SIZE);
            if (bl > 0) {
                sb.append(new String(buffer, 0, bl));
            }
        } while (bl > 0 && buffer[bl - 1] != 0);

        return sb;
    }

    private static String processSegment(String text) throws IOException {
        writeMessage(outputBuffer, text, ENCODING);
        StringBuffer sb = readMessage(inputBuffer);

        writeMessage(outputBuffer, FLUSH_BUFFER_MSG, ENCODING);
        readMessage(inputBuffer);

        return sb.toString();
    }
}
