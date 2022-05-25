import { Credentials } from "../models/credentials";
import { RegisterForm } from "../models/register";
import { myServerUrl } from '../modules/connections';

export const register = async (payload: RegisterForm): Promise<boolean> => {
  const response = await fetch(myServerUrl.value! + '/user/register', {
    headers: {
      'Content-Type': 'application/json'
    },
    method: 'POST',
    body: JSON.stringify(payload)
  });

  return response.status === 200;
}

export const login = async (payload: Credentials): Promise<boolean> => {
  const response = await fetch(myServerUrl.value! + '/user/login', {
    headers: {
      'Content-Type': 'application/json'
    },
    method: 'POST',
    body: JSON.stringify(payload)
  });

  return response.status === 200;
}