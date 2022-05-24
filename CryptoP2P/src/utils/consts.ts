import { BlockMode } from "../models/blockMode";
import * as crypto from 'crypto-js';

export const supportedModes: BlockMode[] = [
  {
    label: 'CBC',
    value: crypto.mode.CBC
  },
  {
    label: 'CFB',
    value: crypto.mode.CFB
  },
  {
    label: 'CTR',
    value: crypto.mode.CTR
  },
  {
    label: 'ECB',
    value: crypto.mode.ECB
  },
  {
    label: 'OFB',
    value: crypto.mode.OFB
  },
];