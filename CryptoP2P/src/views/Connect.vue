<script setup lang="ts">
  import { ref } from 'vue';
  import { HubConnectionBuilder } from '@microsoft/signalr'
  import { myConnection, myServerUrl } from '../modules/connections';
  
  import router from '../router';

  const address = ref<string>();
  const connectWithMyServer = async () => {
    const connection = new HubConnectionBuilder()
      .withUrl(address.value + '/chat')
      .build();
    
    await connection.start();
    myConnection.value = connection;
    myServerUrl.value = address.value;

    router.push('/register');
  }
</script>

<template>
  <div>
    Connect with your own Server
    <input type="text" placeholder="Your server's address" v-model="address"/>
    <button @click="connectWithMyServer" type="button">Submit</button>
  </div>
</template>