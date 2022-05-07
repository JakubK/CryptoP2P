import { HubConnection } from "@microsoft/signalr";
import { Ref, ref } from "vue";

export const myServerUrl = ref<string>();
export const myConnection:Ref<HubConnection | undefined> = ref(undefined);
export const peerConnection:Ref<HubConnection | undefined> = ref(undefined);