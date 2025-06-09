const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();

// Receive new messages in real-time
connection.on("ReceiveMessage", (sender, message) => {
    const messageList = document.getElementById("messagesList");
    const currentUserId = document.getElementById("senderId")?.value;

    if (messageList && currentUserId) {
        const isSender = sender === document.querySelector("input#senderId").dataset.username;

        const messageWrapper = document.createElement("div");
        messageWrapper.className = `mb-3 d-flex flex-column ${isSender ? "align-items-end" : "align-items-start"}`;

        const senderDiv = document.createElement("div");
        senderDiv.className = "small fw-semibold text-muted mb-1";
        senderDiv.textContent = sender;

        const bubbleDiv = document.createElement("div");
        bubbleDiv.className = "px-3 py-2 rounded-4 shadow-sm";
        bubbleDiv.style.maxWidth = "60%";
        bubbleDiv.style.backgroundColor = isSender ? "#b39ddb" : "#a5d6a7";
        bubbleDiv.style.color = "white";
        bubbleDiv.style.fontSize = "0.95rem";
        bubbleDiv.style.lineHeight = "1.4";
        bubbleDiv.textContent = message;

        const timeDiv = document.createElement("div");
        timeDiv.className = "small text-muted mt-1";
        timeDiv.style.fontSize = "0.75rem";
        timeDiv.textContent = new Date().toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });

        messageWrapper.appendChild(senderDiv);
        messageWrapper.appendChild(bubbleDiv);
        messageWrapper.appendChild(timeDiv);

        messageList.appendChild(messageWrapper);
        messageList.scrollTop = messageList.scrollHeight;
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

