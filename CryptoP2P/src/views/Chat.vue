<script setup lang="ts">
import { ref } from 'vue';
import { ChatMessage, DisplayableChatMessage } from '../models/chatMessage';

import { myConnection, myServerUrl } from '../modules/connections';
import { sessionKey } from '../modules/crypto';

import { encrypt, decrypt } from '../utils/crypto';

import { supportedModes } from '../utils/consts';
import { BlockMode } from '../models/blockMode';

const blockModes = ref<BlockMode[]>(supportedModes);
const selectedMode = ref<BlockMode>(blockModes.value[0]);

myConnection.value!.on('ReceiveMessage', (msg: ChatMessage) => {
  msg.message = decrypt(msg.message, sessionKey.value!, selectedMode.value?.value);
  if(!msg.message) {
    msg.message = 'Block Mode Error!';
    msg.type = 'text';
  }

  const displayedMessage = <DisplayableChatMessage>msg;
  displayedMessage.sender = 'Peer'
  messageLog.value?.push(displayedMessage);
});

const message = ref<string>();
const messageLog = ref<Array<DisplayableChatMessage>>([]);

const submitMessage = () => {
  const encryptedMessage = encrypt(message.value!, sessionKey.value!, selectedMode.value?.value);
  const messageObject: ChatMessage = {
    type: 'text',
    message: encryptedMessage
}
  myConnection.value?.invoke('SendMessage', messageObject);
  const loggedMessage: DisplayableChatMessage = {
    ...messageObject,
    message: message.value!,
    sender: 'Me'
  }
  messageLog.value?.push(loggedMessage);
}

const sendingFile = ref(false);
const onFileChange = async (event: any) => {
  sendingFile.value = true;
  const data = new FormData();
  data.append('formFile', event.file);
  const response = await fetch(myServerUrl.value + '/file', {
    method: 'POST',
    body: data
  });
  const jsonResponse = await response.json();
  const message = myServerUrl.value + '/' + jsonResponse.message;
  sendingFile.value = false;

  const messageObject: ChatMessage = {
    type: 'file',
    message
  }

  myConnection.value?.invoke('SendMessage', {
    ...messageObject,
    message: encrypt(message, sessionKey.value!, selectedMode.value?.value)
  });
  const loggedMessage: DisplayableChatMessage = {
    ...messageObject,
    sender: 'Me'
  }
  messageLog.value?.push(loggedMessage);
}
</script>

<template>
  <div>
    <el-scrollbar height="400px">
      <template v-for="(message, index) in messageLog" :key="index">
        <el-divider content-position="left">{{ message.sender }}</el-divider>
        <div v-if="message.type === 'text'">
          {{ message.message }}
        </div>
        <div v-else>
          <a :href="message.message" download>File</a>
        </div>
      </template>
    </el-scrollbar>
    <el-row>
      <div>
        <el-upload
          v-loading="sendingFile"
          :http-request="onFileChange"
          :show-file-list="false"
          action=""
          drag
          :auto-upload="true">
          <el-icon class="el-icon--upload">
            <svg viewBox="0 0 1024 1024" xmlns="http://www.w3.org/2000/svg" data-v-78e17ca8=""><path fill="currentColor" d="M96 896a32 32 0 0 1-32-32V160a32 32 0 0 1 32-32h832a32 32 0 0 1 32 32v704a32 32 0 0 1-32 32H96zm315.52-228.48-68.928-68.928a32 32 0 0 0-45.248 0L128 768.064h778.688l-242.112-290.56a32 32 0 0 0-49.216 0L458.752 665.408a32 32 0 0 1-47.232 2.112zM256 384a96 96 0 1 0 192.064-.064A96 96 0 0 0 256 384z"></path></svg>
          </el-icon>
          <div class="el-upload__text">
            Drop file here or <em>click to upload</em>
          </div>
        </el-upload>
      </div>
      <div>
        <el-row>
          <el-select v-model="selectedMode" size="large">
            <el-option v-for="mode in blockModes"
              :label="mode.label"
              :value="mode"
              :key="mode.label"/>
          </el-select>
          <el-input v-model="message" type="text"/>
          <el-button @click="submitMessage">Submit message</el-button>
        </el-row>
      </div>
    </el-row>
  </div>
</template>