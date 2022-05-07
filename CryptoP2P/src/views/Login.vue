<script setup lang="ts">
  import { ref } from 'vue';
  import { myServerUrl } from '../modules/connections';

  import router from '../router';

  const userName = ref<string>();
  const password = ref<string>();

  const login = async () => {
    const payload = {
      userName : userName.value,
      password: password.value
    }
    const response = await fetch(myServerUrl.value! + '/user/login', {
      headers: {
        'Content-Type': 'application/json'
      },
      method: 'POST',
      body: JSON.stringify(payload)
    });
    if(response.status === 200) {
      router.push('/lobby');
    } else {
      userName.value = password.value = '';
    }
  }

</script>

<template>
  <div>
    Login to your account to be able to start Chat
    <input type="text" placeholder="Username" v-model="userName"/>
    <input type="password" placeholder="Password" v-model="password"/>
    <button @click="login" type="button">Submit</button>
  </div>
</template>
