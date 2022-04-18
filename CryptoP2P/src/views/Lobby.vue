<script setup lang="ts">
import { HubConnectionBuilder } from '@microsoft/signalr'
import { ref } from 'vue';

import { myConnection, peerConnection } from '../modules/connections';

const myServerUrl = ref<string>();
const localServerReached = ref<boolean>();
localServerReached.value = false;

const connectWithMySignalR = async () => {
  myConnection.value = new HubConnectionBuilder()
    .withUrl(myServerUrl.value!)
    .build();
  await myConnection.value.start();

  localServerReached.value = true;
  myConnection.value.on('PeerRequest', async chatEndpoint => {
    peerServerUrl.value = chatEndpoint;
    await connectWithPeerSignalR(false);
  })
}

const peerServerUrl = ref<string>();
const peerServerReached = ref<boolean>();
peerServerReached.value = false;

const messageLog = ref<Array<any>>();
messageLog.value = [];

const connectWithPeerSignalR = async (autoInitConversation: boolean = true) => {
  peerConnection.value = new HubConnectionBuilder()
    .withUrl(peerServerUrl.value!)
    .build();
  await peerConnection.value.start();
  //wait for acceptance
  peerServerReached.value = true;
  myConnection.value!.on('SendMessage', message => {
    //append text from Peer
    messageLog.value?.push('Peer: ' + message);
  });

  if(autoInitConversation)
    peerConnection.value.invoke('InitConversation', myServerUrl.value);
}

const message = ref<string>();
const submitMessage = () => {
  peerConnection.value!.invoke('SendMessage', message.value);
  messageLog.value?.push('Me: ' + message.value);
  message.value = '';
}

</script>

<template>
  <div v-if="!localServerReached">
    Its lobby:<br>
    <input v-model="myServerUrl" type="text" placeholder="Pass in your SignalR Server address" />
    <button @click="connectWithMySignalR">Connect with my SignalR Server</button>
  </div>
  <div v-else-if="!peerServerReached">
    Connected with my: {{ myServerUrl }} <br>
    <input v-model="peerServerUrl" type="text" placeholder="Pass in peer SignalR Server address" />
    <button @click="connectWithPeerSignalR()">Connect with peer SignalR Server</button>
  </div>
  <div v-show="peerServerReached">
    <input v-model="message" type="text"/>
    <button @click="submitMessage">Submit message</button>
    <div id="chat">
      <div v-for="(message, index) in messageLog" :key="index">
        {{ message }}
      </div>
    </div>
  </div>
</template>