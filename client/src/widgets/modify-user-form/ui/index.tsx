import React from 'react'
import { Input } from 'shared/components';

const ModifyUserForm = () => {
  return (
    <form>
        <Input type='text' label='ФИО Пользователя'/>
        <Input type='text' label='Название организации'/>
        <Input type='text' label='Отделение организации'/>
        <Input type='text' label='Деятельность организации'/>
        <Input type='text' label='Является ли админом' />
    </form>
  )
}

export default ModifyUserForm;