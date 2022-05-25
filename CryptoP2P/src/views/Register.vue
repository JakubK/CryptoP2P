<script setup lang="ts">
  import { ref } from 'vue';
  import { RegisterForm } from '../models/register';
  import { register } from '../services/user';
  import router from '../router';

  const userName = ref<string>();
  const password = ref<string>();
  const passwordConfirm = ref<string>();

  const submitRegister = async () => {
    const payload: RegisterForm = {
      userName: userName.value!,
      password: password.value!,
      passwordConfirm: passwordConfirm.value!
    };

    const success = await register(payload);
    if(success)
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
    <button @click="submitRegister" type="button">Create Account</button>
  </div>
</template>