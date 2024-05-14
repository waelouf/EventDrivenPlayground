pipeline {
  agent any
  stages {
    stage('checkout code') {
      steps {
        git(url: 'https://github.com/waelouf/EventDrivenPlayground/', branch: 'main')
      }
    }

    stage('build') {
      steps {
        sh 'dotnet restore'
      }
    }

  }
}