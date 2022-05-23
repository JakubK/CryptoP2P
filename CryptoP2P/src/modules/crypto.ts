import { Ref, ref } from "vue";
import { SessionKey } from "../models/sessionKey";

export const sessionKey: Ref<SessionKey | undefined> = ref<SessionKey>();