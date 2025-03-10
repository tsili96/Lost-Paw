// Create a connection to the SignalR hub
const connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

// This will run when the server sends a message to the group (chat)
connection.on("ReceiveMessage", (sender, message) => {
    // Display the incoming message
    const messageElement = document.createElement("div");
    messageElement.innerText = `${sender}: ${message}`;
    document.getElementById("messagesList").appendChild(messageElement);
});

// Start the connection
connection.start().then(() => {
    console.log("Connected to SignalR hub");

    // Get the chat ID (either from the ViewModel or other methods)
    const chatId = document.getElementById('chatId').value;  // Changed to get the chatId dynamically

    // Join the chat group
    connection.invoke("JoinChat", chatId)
        .catch(err => console.error("Error joining chat: ", err));
}).catch(err => console.error("Error starting connection: ", err));

// Send message function
document.getElementById("sendMessageBtn").addEventListener("click", () => {
    const message = document.getElementById("messageInput").value;
    const chatId = document.getElementById("chatId").value;
    const senderId = document.getElementById("senderId").value; // Βεβαιώσου ότι υπάρχει

    if (!message.trim()) return;

    connection.invoke("SendMessage", chatId, senderId, message)
        .catch(err => console.error("Error sending message: ", err));

    document.getElementById("messageInput").value = "";
});

