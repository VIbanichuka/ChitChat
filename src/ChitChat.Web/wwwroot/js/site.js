const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chitChatHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

connection.on("ReceiveMessageAsync", (message) => {

    var newMessage = $('<div class="outgoing_msg">' +
        '<div class="msg-box sent_msg">' +
        '<p>' + message + '</p>' +
        '</div>' +
        '</div>');

    if (user) {
        var newMessage = $('<div class="outgoing_msg">' +
            '<div class="msg-box sent_msg">' +
            '<p>' + message + '</p>' +
            '</div>' +
            '</div>');
    } else {
        var newMessage = $('<div class="incoming_msg">' +
            '<div class="msg-box received_msg">' +
            '<div class="received_withd_msg">' +
            '<p>' + message + '</p>' +
            '</div>' +
            '</div>' +
            '</div>');
    }
    $('.msg_history').append(newMessage);
    $('#txt_message').val('');
});

$('#btn_send').on('click', function () {
    var message = $('#txt_message').val();
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
