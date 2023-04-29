# Raiders

---

[![플레이핵심완성](https://user-images.githubusercontent.com/46564046/233078635-f2e0a485-f1a0-4e0a-b409-a18958122472.gif)](https://youtu.be/kSDWP_GfiOQ)


<이미지 클릭시 유튜브 전체 영상 재생>

---

## 프로젝트 소개  |  엔터더건전의 리소스를 활용하여 속도감 있는 탑다운뷰 슈팅 게임 구현
- 게임 장르 : 탑다운 2D 슈팅
- 목표 : 다수의 오브젝트가 등장하면서도 속도감 있는 최적화 된 게임 제작
- 플랫폼 : Windows, Mac
- 사용 엔진과 언어 : Unity, C#


## 목표를 위한 구현사항

--- 

### 오브젝트 풀링 (![구현예시영상]([https://youtu.be/kSDWP_GfiOQ?t=39]))


![풀링_용량최적화](https://user-images.githubusercontent.com/46564046/235311704-d339b2c3-2948-469e-b3e9-0d7056496190.gif)

- 게임 내 중점적으로 생성 / 소멸할 적과 총알 객체에 대한 오브젝트 풀링 적용




### 길찾기 함수 콜 최소화 (![구현예시영상]([https://youtu.be/kSDWP_GfiOQ?t=143]))


![길찾기_용량최적화](https://user-images.githubusercontent.com/46564046/235311817-ffe93472-bddd-450d-aa97-f68bf0f40e0b.gif)

- 코루틴 사용 
- 캐릭터 이동 속도, 필드의 장애물 등을 고려하여 swich문을 통해 필요한 경우에만 길찾기 함수 콜 감소
