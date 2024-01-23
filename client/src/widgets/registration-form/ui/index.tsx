import React, { useState } from 'react'
import { Button, Input } from 'shared/components';

import styles from './RegistrationForm.module.scss'

interface RegistrationFormProps {
  changeForm: Function,
  onSubmit: Function
}


const RegistrationForm:React.FC<RegistrationFormProps> = props => {
  const {changeForm, onSubmit} = props

  const [formData,setFormData] = useState({
    login: '',
    password: '',
    repeatedPassword: '',
    name: '',
    orgName: '',
    orgBranch: '',
    orgActivity: ''
  })




  const handleSubmit = (event: React.FormEvent) => {
    event.preventDefault()
    onSubmit({
      login: formData.login,
      password: formData.password,
      role: 'Admin',
      name: formData.name,
      orgName: formData.orgName,
      orgBranch: formData.orgBranch,
      orgActivity: formData.orgActivity
    })
  }

  return (
    <form className={styles.form} onSubmit={(event) => handleSubmit(event)}>
        <Input type='text' label='Логин' value={formData.login} onChange={(value: string) => setFormData({...formData, login: value})}/>
        <Input type='password' label='Пароль' value={formData.password} onChange={(value: string) => setFormData({...formData, password: value})}/>
        <Input type='password' label='Повторите пароль' value={formData.repeatedPassword} onChange={(value: string) => setFormData({...formData, repeatedPassword: value})}/>
        <Input type='text' label='ФИО пользователя' value={formData.name} onChange={(value: string) => setFormData({...formData, name: value})}/>
        <Input type='text' label='Название организации' value={formData.orgName} onChange={(value: string) => setFormData({...formData, orgName: value})}/>
        <Input type='text' label='Отделение организации' value={formData.orgBranch} onChange={(value: string) => setFormData({...formData, orgBranch: value})}/>
        <Input type='text' label='Деятельность организации' value={formData.orgActivity} onChange={(value: string) => setFormData({...formData, orgActivity: value})}/>
        <div className={styles.buttonsContainer}>
            <Button onClick={() => {}} isFilled className={styles.formButton} type='submit'>Зарегистрироваться</Button>
            <Button className={styles.formButton} onClick={() => changeForm()} isColoredText>Авторизироваться</Button>
        </div>
    </form>
  )
}

export default RegistrationForm;