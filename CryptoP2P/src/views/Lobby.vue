<script setup lang="ts">
import { ref } from 'vue';
import { ChatMessage } from '../models/chatMessage';

import { myConnection, myServerUrl } from '../modules/connections';


myConnection.value!.on('ConversationStarted', () => {
  peerServerReached.value = true;
})
myConnection.value!.on('ReceiveMessage', (msg:ChatMessage) => {
  messageLog.value?.push(msg);
});

const peerServerUrl = ref<string>();
const peerServerReached = ref<boolean>();
peerServerReached.value = false;

const connectWithPeerSignalR = () => {
  myConnection.value?.invoke('InitConversation', peerServerUrl.value + "/chat");
};

const message = ref<string>();
const messageLog = ref<Array<ChatMessage>>();
messageLog.value = [];

const submitMessage = () => {
  const messageObject: ChatMessage = {
    blockMode: '',
    type: 'text',
    message: message.value!
  }
  myConnection.value?.invoke('SendMessage', messageObject);
  const loggedMessage: ChatMessage = {
    ...messageObject,
    message: 'Me: ' + messageObject.message
  }
  messageLog.value?.push(loggedMessage);
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

  const jsonResponse = await response.json();
  const messageObject: ChatMessage = {
    blockMode: '',
    type: 'file',
    message: myServerUrl.value + '/' + jsonResponse.message
  }
  myConnection.value?.invoke('SendMessage', messageObject)
  messageLog.value?.push(messageObject);
}

</script>

<template>
  <div v-if="!peerServerReached">
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
      <template v-for="(message, index) in messageLog" :key="index">
        <div v-if="message.type === 'text'">
          {{ message.message }}
        </div>
        <div v-else>
          <a :href="message.message" download>File</a>
        </div>
      </template>
    </div>
  </div>
</template>