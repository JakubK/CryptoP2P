<script setup lang="ts">
import { HubConnectionBuilder } from '@microsoft/signalr'
import { ref } from 'vue';
import { ChatMessage } from '../models/chatMessage';

import { myConnection } from '../modules/connections';

const myServerUrl = ref<string>();
const myServerActive = ref<boolean>();
myServerActive.value = false;

const connectWithServer = async () => {
  myConnection.value = new HubConnectionBuilder()
    .withUrl(myServerUrl.value! + '/chat')
    .build();

  await myConnection.value.start();

  myConnection.value.invoke('ConnectOwner');
  myConnection.value.on('ConnectOwnerResponse', () => {
    myServerActive.value = true;
  });
  myConnection.value.on('ConversationStarted', () => {
    peerServerReached.value = true;
  })
  myConnection.value.on('ReceiveMessage', (msg:ChatMessage) => {
    messageLog.value?.push('Peer: ' + msg.message);
  });
}

const peerServerUrl = ref<string>();
const peerServerReached = ref<boolean>();
peerServerReached.value = false;

const connectWithPeerSignalR = () => {
  myConnection.value?.invoke('InitConversation', peerServerUrl.value + "/chat");
};

const message = ref<string>();
const messageLog = ref<Array<any>>();
messageLog.value = [];

const submitMessage = () => {
  messageLog.value?.push('Me: ' + message.value);
  myConnection.value?.invoke('SendMessage', message.value);
}

const onFileChange = async (event: Event) => {
  const target = event.target as HTMLInputElement;

  const data = new FormData();
  if(target.files === null )
    return;
  data.append('formFile', target.files[0]);
  const response = await fetch(myServerUrl.value + '/file', {
    method: 'POST',
    body: data
  });
  const json = await response.json();
  console.log(json);
}

</script>

<template>
  <div v-if="!myServerActive">
    Its lobby:<br>
    <input v-model="myServerUrl" type="text" placeholder="Pass in your SignalR Server address" />
    <button @click="connectWithServer">Connect with my SignalR Server</button>
  </div>
  <div v-else-if="!peerServerReached">
    Connected with my: {{ myServerUrl }} <br>
    <input v-model="peerServerUrl" type="text" placeholder="Pass in peer SignalR Server address" />
    <button @click="connectWithPeerSignalR()">Connect with peer SignalR Server</button>
  </div>
  <div v-else>
    Conversation Started!!!!!
    <input v-model="message" type="text"/>
    <button @click="submitMessage">Submit message</button>
    <input @change="onFileChange" type="file"/>
    <div>
      <div v-for="(message, index) in messageLog" :key="index">
        {{ message }}
      </div>
    </div>
  </div>
</template>