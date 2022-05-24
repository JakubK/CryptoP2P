<script setup lang="ts">
import { ref } from 'vue';
import { ChatMessage } from '../models/chatMessage';

import { myConnection, myServerUrl } from '../modules/connections';
import { sessionKey } from '../modules/crypto';

import { encrypt, decrypt } from '../utils/crypto';

import { supportedModes } from '../utils/consts';
import { BlockMode } from '../models/blockMode';

const selectedMode = ref<BlockMode>();
const blockModes = ref<BlockMode[]>();
blockModes.value = supportedModes;

selectedMode.value = blockModes.value[0];

myConnection.value!.on('ReceiveMessage', (msg:ChatMessage) => {
  msg.message = decrypt(msg.message, sessionKey.value!, selectedMode.value?.value);
  if(!msg.message)
    msg.message = 'Block Mode Error!';
  messageLog.value?.push(msg);
});

const message = ref<string>();
const messageLog = ref<Array<ChatMessage>>();
messageLog.value = [];

const submitMessage = () => {
  const encryptedMessage = encrypt(message.value!, sessionKey.value!, selectedMode.value?.value);
  const messageObject: ChatMessage = {
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
    type: 'file',
    message
  }
  messageLog.value?.push(messageObject);
  myConnection.value?.invoke('SendMessage', {
    ...messageObject,
    message: encrypt(message, sessionKey.value!, selectedMode.value?.value)
  });
}
</script>

<template>
  <div>
    Conversation Started!!!!!
    <input v-model="message" type="text"/>
    <button @click="submitMessage">Submit message</button>
    <input @change="onFileChange" type="file"/>
    <select v-model="selectedMode">
      <option v-for="mode in blockModes" :value="mode" :key="mode.label">
        {{ mode.label  }}
      </option>
    </select>
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