import React from 'react'

import styles from './RadioGroup.module.scss'

interface RadioGroupProps {
  buttons: {id: string, value: string, label: string}[]
  name: string
  value: string
  onChange: Function
}

const RadioGroup: React.FC<RadioGroupProps> = props => {
  const {
    buttons,
    name,
    value,
    onChange
  } = props

  return (
    <div className={styles.buttonsGroup}>
      {buttons.map(button => 
        <div className={styles.radioButton}>
          <input 
            type="radio" 
            id={button.id} 
            name={name} 
            value={button.value} 
            checked={value === button.value ? true : false}
            onChange={() => onChange(button.value)}
          />
          <label htmlFor={button.id}>{button.label}</label>
        </div>
      )}
    </div>
  )
}

export default RadioGroup