<?xml version="1.0" encoding="UTF-8"?>
<tileset name="QABoxObjects" tilewidth="1920" tileheight="1080" tilecount="12" columns="0">
 <grid orientation="orthogonal" width="1" height="1"/>
 <tile id="0">
  <properties>
   <property name="spawner" type="bool" value="true"/>
   <property name="visible" type="bool" value="false"/>
  </properties>
  <image width="500" height="500" source="Spawner.png"/>
 </tile>
 <tile id="1">
  <image width="400" height="600" source="Panel_DontPlay.png"/>
 </tile>
 <tile id="2">
  <image width="400" height="600" source="Panel_Options.png"/>
 </tile>
 <tile id="3">
  <image width="400" height="600" source="Panel_Play.png"/>
 </tile>
 <tile id="4">
  <properties>
   <property name="active" type="bool" value="true"/>
   <property name="camcollider" type="bool" value="true"/>
   <property name="forbid" value=""/>
   <property name="lockid" type="int" value="0"/>
   <property name="visible" type="bool" value="false"/>
  </properties>
  <image width="500" height="500" source="CamLock.png"/>
 </tile>
 <tile id="5">
  <properties>
   <property name="colliderground" type="bool" value="true"/>
   <property name="panel" value=""/>
  </properties>
  <image width="500" height="500" source="Ladder.png"/>
 </tile>
 <tile id="6">
  <properties>
   <property name="Name" value=""/>
   <property name="NextLevel" value=""/>
   <property name="SpeScript" value=""/>
  </properties>
  <image width="500" height="500" source="LevelVals.png"/>
 </tile>
 <tile id="7">
  <properties>
   <property name="active" type="bool" value="true"/>
   <property name="camcollider" type="bool" value="false"/>
   <property name="unlockid" type="int" value="0"/>
   <property name="visible" type="bool" value="false"/>
  </properties>
  <image width="500" height="500" source="CamUnlock.png"/>
 </tile>
 <tile id="8">
  <properties>
   <property name="collider" type="bool" value="true"/>
   <property name="face" value=""/>
  </properties>
  <image width="500" height="500" source="Block.png"/>
 </tile>
 <tile id="9">
  <image width="400" height="600" source="Panel_Next.png"/>
 </tile>
 <tile id="10">
  <properties>
   <property name="Background" type="bool" value="true"/>
  </properties>
  <image width="1920" height="1080" source="Background1.png"/>
 </tile>
 <tile id="11">
  <properties>
   <property name="Background" type="bool" value="true"/>
  </properties>
  <image width="1920" height="1080" source="Background2.png"/>
 </tile>
</tileset>
