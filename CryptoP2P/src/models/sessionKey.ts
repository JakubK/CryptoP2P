import * as crypto from 'crypto-js';

export interface SessionKey {
  key: string;
  iv: string;
}