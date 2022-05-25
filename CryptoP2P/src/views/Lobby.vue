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
  <div>
    Connected with my: {{ myServerUrl }} <br>
    <input v-model="peerServerUrl" type="text" placeholder="Pass in peer SignalR Server address" />
    <button @click="connectWithPeerSignalR()">Connect with peer SignalR Server</button>
  </div>
</template>