<script setup lang="ts">
  import { ref } from 'vue';
  import { RegisterForm } from '../models/register';
  import { register } from '../services/user';
  import { ElMessage } from 'element-plus';

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
      ElMessage({
        message: 'Congrats, new account created. Now try to log in',
        type: 'success',
      })
    else
      ElMessage({
        message: 'User already exists.',
        type: 'warning',
      })

  };
</script>

<template>
  <el-form>
    <el-form-item label="User name">
      <el-input v-model="userName"/>
    </el-form-item>
    <el-form-item label="User password">
      <el-input type="password" v-model="password" show-password/>
    </el-form-item>
    <el-form-item label="Repeat password">
      <el-input type="password" v-model="passwordConfirm" show-password/>
    </el-form-item>
    <el-form-item>
      <el-button type="primary" @click="submitRegister">Register</el-button>
    </el-form-item>
  </el-form>
</template>