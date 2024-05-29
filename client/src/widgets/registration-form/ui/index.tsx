import React, { useState } from 'react'
import { Button, Input } from 'shared/components';

import styles from './RegistrationForm.module.scss'
import { useFormik } from 'formik';
import { AxiosError } from 'axios';
import { UserRegistration } from '../model/registrationModel';

interface RegistrationFormProps {
  changeForm: Function,
  onSubmit: Function
}


const RegistrationForm:React.FC<RegistrationFormProps> = props => {
  const {changeForm, onSubmit} = props
  const [error, setError] = useState('')

  const validate = (values: UserRegistration & {repeatedPassword: string}) => {
    const errors: Partial<UserRegistration & {repeatedPassword: string}> = {};
  
    if (!values.login) {
      errors.login = 'Заполните поле';
    } else if (!/^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$/i.test(values.login)) {
      errors.login = 'Некорректный логин';
    }
  
   if(values.password.length < 7) {
    errors.password = 'Пароль должен быть не меньше 7 символов'
   }

   if(values.password !== values.repeatedPassword) {
    errors.repeatedPassword = 'Пароли не совпадают'
   }
  
    return errors;
  };

  const formik = useFormik({
    initialValues: {
      login: '',
      password: '',
      role: '',
      repeatedPassword: '',
      name: '',
      orgName: '',
      orgBranch: '',
      orgActivity: ''
    },
    validate,
    onSubmit: values => {
      onSubmit({...values, role: 'Admin'
      })
      .catch((err: AxiosError) => {
        if(err.response?.status === 400) {
          setError('Неправильный логин или пароль')
        }
      });
    },

  });


  return (
    <form className={styles.form} onSubmit={(event) => {
      event.preventDefault()
      formik.handleSubmit()
    }}>
        <Input type='text' label='Логин' onChange={formik.handleChange('login')} value={formik.values.login} error={formik.errors.login}/>
        <Input type='password' label='Пароль' onChange={formik.handleChange('password')} value={formik.values.password} error={formik.errors.password}/>
        <Input type='password' label='Повторите пароль' onChange={formik.handleChange('repeatedPassword')} value={formik.values.repeatedPassword} error={formik.errors.repeatedPassword}/>
        <Input type='text' label='ФИО пользователя' onChange={formik.handleChange('name')} value={formik.values.name} error={formik.errors.name}/>
        <Input type='text' label='Название организации' onChange={formik.handleChange('orgName')} value={formik.values.orgName} error={formik.errors.orgName}/>
        <Input type='text' label='Отделение организации' onChange={formik.handleChange('orgBranch')} value={formik.values.orgBranch} error={formik.errors.orgBranch}/>
        <Input type='text' label='Деятельность организации' onChange={formik.handleChange('orgActivity')} value={formik.values.orgActivity} error={formik.errors.orgActivity}/>
        <div className={styles.buttonsContainer}>
            <Button onClick={() => {}} isFilled className={styles.formButton} type='submit'>Зарегистрироваться</Button>
            <Button className={styles.formButton} onClick={() => changeForm()} isColoredText>Авторизоваться</Button>
        </div>
    </form>
  )
}

export default RegistrationForm;