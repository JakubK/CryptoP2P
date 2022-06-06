export interface ChatMessage {
  type: string;
  message: string;
}

export interface DisplayableChatMessage extends ChatMessage {
  sender: string;
}