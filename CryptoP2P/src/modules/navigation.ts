import { ref } from "vue";
import router from "../router";

const active = ref(0);
export const useNavigation = () => {
  const stepPaths = [
    '/',
    '/auth',
    '/lobby'
  ]

  const next = () => {
    active.value++;
    router.push(stepPaths[active.value]);
  }

  const reset = () => {
    active.value = 0;
    router.push(stepPaths[active.value]);
  }

  const endConversation = () => {
    
  }

  const current = () => active.value;

  return {
    current,
    next,
    reset
  }
}