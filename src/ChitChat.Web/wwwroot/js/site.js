const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chitChatHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

connection.on("ReceiveMessageAsync", (message) => {
    $('#signalr-message-panel').prepend($('<div />').text(message));
});

$('#btn-broadcast').on('click', function () {
    var message = $('#broadcast').val();
    connection.invoke("BroadcastMessage", message).catch(err => console.error(err.toString()));
});

async function start() {
    try {
        await connection.start();
        console.log('connected');

    } catch (err) {
        console.log(err);
        setTimeout(() => start(), 5000);
    }
};

connection.onclose(async () => {
    await start();
});

start();
