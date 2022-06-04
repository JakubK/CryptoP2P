<template>
  <el-header>
    <el-steps :active="current()" align-center finish-status="success">
      <el-step title="Connect to your server" />
      <el-step title="Sign up / Sign in" />
      <el-step title="Connect with your peer" />
    </el-steps>
  </el-header>
  <el-main>
    <router-view></router-view>
  </el-main>
</template>

<script lang="ts" setup>
  import { useNavigation } from './modules/navigation';
  const { current } = useNavigation();
</script>