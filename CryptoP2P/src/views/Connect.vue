<script setup lang="ts">
  import { onMounted, ref } from 'vue';
  import { HubConnectionBuilder } from '@microsoft/signalr'
  import { myConnection, myServerUrl } from '../modules/connections';
  
  import { useNavigation } from '../modules/navigation';
  const { reset, next } = useNavigation();
  onMounted(() => reset());

  const address = ref<string>();
  const connectWithMyServer = async () => {
    const connection = new HubConnectionBuilder()
      .withUrl(address.value + '/chat')
      .build();
    
    await connection.start();
    myConnection.value = connection;
    myServerUrl.value = address.value;

    next();
  }
</script>

<template>
  <el-container>
    <el-input v-model="address" placeholder="Type Your server's address"/>
    <el-button @click="connectWithMyServer">Connect</el-button>
  </el-container>
</template>