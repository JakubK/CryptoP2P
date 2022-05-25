<script setup lang="ts">
  import { ref } from 'vue';
  import { Credentials } from '../models/credentials';

  import router from '../router';
  import { login } from '../services/user';

  const userName = ref<string>();
  const password = ref<string>();

  const submitLogin = async () => {
    const payload: Credentials = {
      userName : userName.value!,
      password: password.value!
    }
    const success = await login(payload);

    if(success) 
      router.push('/lobby');
    else 
      userName.value = password.value = '';
  }

</script>

<template>
  <div>
    Login to your account to be able to start Chat
    <input type="text" placeholder="Username" v-model="userName"/>
    <input type="password" placeholder="Password" v-model="password"/>
    <button @click="submitLogin" type="button">Submit</button>
  </div>
</template>
