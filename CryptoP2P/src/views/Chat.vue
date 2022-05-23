<script setup lang="ts">
import { ref } from 'vue';
import { ChatMessage } from '../models/chatMessage';

import * as crypto from 'crypto-js';

import { myConnection, myServerUrl } from '../modules/connections';
import { sessionKey } from '../modules/crypto';

import { encrypt, decrypt } from '../utils/crypto';

myConnection.value!.on('ReceiveMessage', (msg:ChatMessage) => {
  msg.message = decrypt(msg.message, sessionKey.value!, crypto.mode.CBC);
  messageLog.value?.push( msg);
});

const message = ref<string>();
const messageLog = ref<Array<ChatMessage>>();
messageLog.value = [];

const submitMessage = () => {
  const encryptedMessage = encrypt(message.value!, sessionKey.value!, crypto.mode.CBC);
  const messageObject: ChatMessage = {
    blockMode: '',
    type: 'text',
    message: encryptedMessage
  }
  myConnection.value?.invoke('SendMessage', messageObject);
  const loggedMessage: ChatMessage = {
    ...messageObject,
    message: 'Me: ' + message.value!
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
  const message = myServerUrl.value + '/' + jsonResponse.message;

  const messageObject: ChatMessage = {
    blockMode: '',
    type: 'file',
    message
  }
  messageLog.value?.push(messageObject);
  myConnection.value?.invoke('SendMessage', {
    ...messageObject,
    message: encrypt(message, sessionKey.value!, crypto.mode.CBC)
  });
}
</script>

<template>
  <div>
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