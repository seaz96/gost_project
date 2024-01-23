import React, { useState } from 'react'
import { Button, Input } from 'shared/components';

import styles from './AuthorizationForm.module.scss'

interface AuthorizationFormProps {
  changeForm: Function,
  onSubmit: Function
}

const AuthorizationForm: React.FC<AuthorizationFormProps> = props => {
  const [login, setLogin] = useState('');
  const [password, setPassword] = useState('');
  const {changeForm, onSubmit} = props

  const handleSubmit = () => {
    onSubmit({login, password})
  }

  return (
    <form className={styles.form} onSubmit={(event) => {
      event.preventDefault()
      handleSubmit()
      }}>
      <Input type='text' label='Логин' value={login} onChange={(value: string) => setLogin(value)}/>
      <Input type='password' label='Пароль' value={password} onChange={(value: string) => setPassword(value)}/>
      <div className={styles.buttonsContainer}>
          <Button onClick={() => {}} isFilled className={styles.formButton} type='submit'>Войти</Button>
          <Button className={styles.formButton} onClick={() => changeForm()} isColoredText>Зарегистрироваться</Button>
      </div>
</form>
  )
}

export default AuthorizationForm;