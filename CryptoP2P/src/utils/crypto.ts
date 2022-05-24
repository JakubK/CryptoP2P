import * as crypto from 'crypto-js';
import { SessionKey } from '../models/sessionKey';

export const generateSessionKey = (): SessionKey => {
  const key = crypto.lib.WordArray.random(16).toString();
  const iv = crypto.lib.WordArray.random(16).toString();
  return {
    key, iv
  }
}

export const encrypt = (message: string, key: SessionKey, mode: any): string => {
  const encrypted = crypto.AES.encrypt(message, key.key, {
    iv: crypto.enc.Hex.parse(key.iv),
    mode
  }).toString();
  return encrypted;
}

export const decrypt = (cipher: string, key: SessionKey, mode: any): string => {
  const decrypted = crypto.AES.decrypt(cipher, key.key, {
    iv: crypto.enc.Hex.parse(key.iv),
    mode
  }).toString(crypto.enc.Utf8);
  return decrypted;
}