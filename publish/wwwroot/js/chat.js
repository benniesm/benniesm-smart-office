"use strict";

const appOrigin3 = window.location.origin;
const apiAddress3 = appOrigin3 + '/api/ChatMessage';

const showChats = (user, message, sound) => {
    let msgPack = JSON.parse(message);
    let msg = msgPack.msg.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");

    let newChat = document.createElement('div');
    newChat.className += 'chat-card';
    let sender = document.createElement('span');
    sender.className += 'chat-sender';
    let timestamp = document.createElement('span');
    timestamp.className += 'chat-time';
    let chatMsg = document.createElement('span');
    chatMsg.className += 'chat-msg'

    let msgTime = new Date(msgPack.time),
    month = '' + (msgTime.getMonth() + 1),
    day = '' + msgTime.getDate(),
    year = msgTime.getFullYear(),
    hour = '' + msgTime.getHours(),
    minute = '' + msgTime.getMinutes();

    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;
    if (hour.length < 2) hour = '0' + hour;
    if (minute.length < 2) minute = '0' + minute;

    let timePretty = [day, month, year].join('-')+' '+
       [hour, minute].join(':');

    sender.textContent = user;
    timestamp.textContent = timePretty;
    chatMsg.textContent = msg;

    newChat.appendChild(sender);
    newChat.appendChild(chatMsg);
    newChat.appendChild(timestamp);
    let messagesContent =  document.getElementById('messages-content');
    messagesContent.appendChild(newChat);
    messagesContent.scrollTop = messagesContent.scrollHeight;
    if (sound) {
        const audio = new Audio(appOrigin3 + '/media/system-fault-518.mp3');
        audio.play();
    }
}

let oldMsgs = [];
    //fetch from db
fetch(apiAddress3)
.then(response => response.json()
    .then(data => ({
        status: response.status,
        body: data
    })))
.then(response => {
    //console.log(response);
    if (response.status === 200) {
        response.body.forEach(r => {
            let message = JSON.stringify({
                msg: r.message,
                time: r.timeIn
            });
            showChats(r.owner, message, false);
        });
    }
})
.catch(error => {
    console.log(error);
});

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    showChats(user, message, true);
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    let msgBundle = JSON.stringify({
        time: new Date(),
        msg: message
    });
    connection.invoke("SendMessage", user, msgBundle).catch(function (err) {
        return console.error(err.toString());
    });
    document.getElementById("messageInput").value = "";
    
    event.preventDefault();
});
