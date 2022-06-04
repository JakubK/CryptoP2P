<script setup lang="ts">
  import { ref } from 'vue';
  import { Credentials } from '../models/credentials';

  import { login } from '../services/user';

  import { useNavigation } from '../modules/navigation';
import { ElMessage } from 'element-plus';
  const { next } = useNavigation();

  const userName = ref<string>();
  const password = ref<string>();

  const submitLogin = async () => {
    const payload: Credentials = {
      userName : userName.value!,
      password: password.value!
    }
    const success = await login(payload);

    if(success) 
      next();
    else {
      ElMessage({
        message: 'Wrong credentials passed',
        type: 'warning',
      })
      userName.value = password.value = '';
    }
  }

</script>

<template>
  <el-form>
    <el-form-item label="User name">
      <el-input v-model="userName"/>
    </el-form-item>
    <el-form-item label="User password">
      <el-input type="password" v-model="password" show-password/>
    </el-form-item>
    <el-form-item>
      <el-button type="primary" @click="submitLogin">Log in</el-button>
    </el-form-item>
  </el-form>
</template>
