<script setup lang="ts">
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr'
import { ref } from 'vue';

let myConnection: HubConnection;

const myServerUrl = ref<string>();
const localServerReached = ref<boolean>();
localServerReached.value = false;

const connectWithMySignalR = async () => {
  myConnection = new HubConnectionBuilder()
    .withUrl(myServerUrl.value!)
    .build();
  await myConnection.start();
  localServerReached.value = true;
}

let peerConnection: HubConnection;

const peerServerUrl = ref<string>();
const peerServerReached = ref<boolean>();
peerServerReached.value = false;

const messageLog = ref<Array<any>>();
messageLog.value = [];

const connectWithPeerSignalR = async () => {
  peerConnection = new HubConnectionBuilder()
    .withUrl(peerServerUrl.value!)
    .build();
  await peerConnection.start();
  //wait for acceptance
  peerServerReached.value = true;

  myConnection.on('DeliverMessage', message => {
    //append text from Peer
    messageLog.value?.push('\nPeer: ' + message);
  });
}


const message = ref<string>();
const submitMessage = () => {
  peerConnection.invoke('SendMessage', message.value);
  messageLog.value?.push('\Me: ' + message.value);
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
    <button @click="connectWithPeerSignalR">Connect with peer SignalR Server</button>
  </div>
  <div v-show="peerServerReached">
    <div id="chat">
      <div v-for="(message, index) in messageLog" :key="index">
        {{ message }}
      </div>
    </div>
    <input v-model="message" type="text"/>
    <button @click="submitMessage">Submit message</button>
  </div>
</template>