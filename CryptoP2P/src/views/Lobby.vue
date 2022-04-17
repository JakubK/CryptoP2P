<script setup lang="ts">
import signalR from '@microsoft/signalr'
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chat")
    .build();
</script>

<template>
  Its lobby:
</template>