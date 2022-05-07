<script setup lang="ts">
  import { ref } from 'vue';
  import { myServerUrl } from '../modules/connections';

  import router from '../router';

  const userName = ref<string>();
  const password = ref<string>();
  const passwordConfirm = ref<string>();

  const register = async () => {

    const payload = {
      userName: userName.value,
      password: password.value,
      passwordConfirm: passwordConfirm.value
    };

    const response = await fetch(myServerUrl.value! + '/user/register', {
      headers: {
        'Content-Type': 'application/json'
      },
      method: 'POST',
      body: JSON.stringify(payload)
    });
    if(response.status === 200)
      router.push('/login')
    else
      userName.value = password.value = passwordConfirm.value = '';
  };
</script>

<template>
  <div>
    Register Page
    <input type="text" placeholder="Username" v-model="userName"/>
    <input type="password" placeholder="Password" v-model="password"/>
    <input type="password" placeholder="Password Confirm" v-model="passwordConfirm"/>
    <button @click="register" type="button">Create Account</button>
  </div>
</template>