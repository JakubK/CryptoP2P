<script setup lang="ts">
import { ref } from 'vue';
import { myConnection, myServerUrl } from '../modules/connections';
import { generateSessionKey } from '../utils/crypto';
import router from '../router';
import { sessionKey } from '../modules/crypto';

const peerServerUrl = ref<string>('');

myConnection.value!.on('ConversationStarted', (key) => {
  if(key)
    sessionKey.value = key;
  //redirect to chat
  router.push('/chat'); 
})

const connectWithPeerSignalR = () => {
  // Generate session key
  const generatedSessionKey = generateSessionKey();
  sessionKey.value = generatedSessionKey;
  myConnection.value?.invoke('InitConversation', peerServerUrl.value + "/chat", generatedSessionKey);
};
</script>

<template>
    Connected with my: {{ myServerUrl }} <br>
    <el-form>
      <el-form-item label="Peer server address">
        <el-input v-model="peerServerUrl"/>
      </el-form-item>
      <el-form-item>
        <el-button @click="connectWithPeerSignalR">Start Conversation</el-button>
      </el-form-item>
    </el-form>
</template>