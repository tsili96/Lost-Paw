const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();

// Receive new messages in real-time
connection.on("ReceiveMessage", (sender, message) => {
    const messageList = document.getElementById("messagesList");
    if (messageList) {
        const messageElement = document.createElement("div");
        messageElement.innerText = `${sender}: ${message}`;
        messageList.appendChild(messageElement);
    }
});

// Update chat list in real-time
connection.on("UpdateChatList", (chatId, sender, message) => {
    const chatElement = document.querySelector(`.chat-item[data-chat-id='${chatId}']`);
    if (chatElement) {
        chatElement.querySelector(".last-message").innerText = `${sender}: ${message}`;
    } else {
        const chatList = document.querySelector(".chat-list");
        if (chatList) {
            const newChatItem = document.createElement("div");
            newChatItem.classList.add("chat-item", "border", "rounded", "p-3", "mb-2", "shadow-sm");
            newChatItem.setAttribute("data-chat-id", chatId);
            newChatItem.innerHTML = `
                <a href="/Chat/OpenChat?chatId=${chatId}" class="d-flex justify-content-between align-items-center text-decoration-none text-dark">
                    <div>
                        <strong>${sender}</strong>
                        <p class="last-message mb-0 text-muted">${message}</p>
                    </div>
                </a>
            `;
            chatList.prepend(newChatItem);
        }
    }
});

// Start SignalR connection
connection.start().then(() => {
    const chatIdInput = document.getElementById("chatId");
    if (chatIdInput) {
        connection.invoke("JoinChat", chatIdInput.value).catch(err => console.error("Error joining chat: ", err));
    }
}).catch(err => console.error("Error starting connection: ", err));


// Send message functionality
document.addEventListener("DOMContentLoaded", function () {
    const sendButton = document.getElementById("sendMessageBtn");

    if (!sendButton) return;

    sendButton.addEventListener("click", () => {
        const messageInput = document.getElementById("messageInput");
        const senderIdInput = document.getElementById("senderId");
        const chatIdInput = document.getElementById("chatId");

        if (!messageInput || !senderIdInput || !chatIdInput) return;

        const message = messageInput.value.trim();
        const senderId = senderIdInput.value;
        const chatId = chatIdInput.value;

        if (!message) return;

        connection.invoke("SendMessage", parseInt(chatId), senderId, message)
            .then(() => messageInput.value = "")
            .catch(err => console.error("Error sending message:", err));
    });
});

