<template>
  <el-tabs v-model="activeTab">
    <el-tab-pane label="Register" name="register">
      <Register/>
    </el-tab-pane>
    <el-tab-pane label="Login" name="login">
      <Login/>
    </el-tab-pane>
  </el-tabs>
</template>

<script lang="ts" setup>
  import { ref } from 'vue';
  import Register from './Register.vue';
  import Login from './Login.vue';

  const activeTab = ref('register');
</script>