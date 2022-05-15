<script setup lang="ts">
import { ref } from 'vue';
import { myConnection, myServerUrl } from '../modules/connections';

import router from '../router';

const peerServerUrl = ref<string>();

myConnection.value!.on('ConversationStarted', () => {
  //redirect to chat
  router.push('/chat');
})

const connectWithPeerSignalR = () => {
  myConnection.value?.invoke('InitConversation', peerServerUrl.value + "/chat");
};
</script>

<template>
  <div>
    Connected with my: {{ myServerUrl }} <br>
    <input v-model="peerServerUrl" type="text" placeholder="Pass in peer SignalR Server address" />
    <button @click="connectWithPeerSignalR()">Connect with peer SignalR Server</button>
  </div>
</template>